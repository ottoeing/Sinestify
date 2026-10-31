export interface Genre {
  id: number
  nome: string
}

export interface Emotion {
  id: number
  sentimento: string
}

export interface Music {
  id: number
  nome: string
  cantor: string
  generoId: number
  genero?: Genre | null
  emocaoId: number
  emocao?: Emotion | null
  velocidade: number
}

interface UserCredentials {
  email: string
  senha: string
}

export class ApiError extends Error {
  readonly status: number

  constructor(message: string, status: number) {
    super(message)
    this.name = 'ApiError'
    this.status = status
  }
}

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL?.replace(/\/$/, '') ?? ''

async function request<T>(
  path: string,
  options: RequestInit = {},
  token?: string,
): Promise<T> {
  const headers = new Headers(options.headers)
  if (options.body) headers.set('Content-Type', 'application/json')
  if (token) headers.set('Authorization', `Bearer ${token}`)

  const response = await fetch(`${apiBaseUrl}${path}`, { ...options, headers })
  const contentType = response.headers.get('content-type') ?? ''
  const result: unknown = contentType.includes('application/json')
    ? await response.json()
    : await response.text()

  if (!response.ok) {
    const message = typeof result === 'string' && result.trim()
      ? result
      : `A solicitação falhou (${response.status}).`
    throw new ApiError(message, response.status)
  }

  return result as T
}

function assertSuccessfulMessage(result: unknown) {
  if (typeof result === 'string' && /não foi possível|não encontrada|inválidas/i.test(result)) {
    throw new Error(result)
  }
}

function nameFromToken(token: string) {
  try {
    const payload = token.split('.')[1]
    const base64 = payload.replace(/-/g, '+').replace(/_/g, '/').padEnd(Math.ceil(payload.length / 4) * 4, '=')
    const bytes = Uint8Array.from(atob(base64), (character) => character.charCodeAt(0))
    const claims = JSON.parse(new TextDecoder().decode(bytes)) as Record<string, unknown>
    const name = claims.name ?? claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
    return typeof name === 'string' && name.trim() ? name : 'Sua conta'
  } catch {
    return 'Sua conta'
  }
}

export async function authenticate(credentials: UserCredentials) {
  const result = await request<unknown>('/login/auth', {
    method: 'POST',
    body: JSON.stringify(credentials),
  })

  const token = typeof result === 'string' ? result.trim().replace(/^"|"$/g, '') : ''
  if (token.split('.').length !== 3) {
    throw new Error(typeof result === 'string' && result.trim() ? result : 'Não foi possível entrar. Verifique suas credenciais.')
  }

  return { token, name: nameFromToken(token) }
}

export async function registerUser(user: UserCredentials & { nome: string }) {
  const result = await request<unknown>('/usuarios/registrar', {
    method: 'POST',
    body: JSON.stringify(user),
  })
  assertSuccessfulMessage(result)
  if (typeof result === 'string' && !result.toLocaleLowerCase('pt-BR').includes('registrado com sucesso')) {
    throw new Error(result || 'Não foi possível criar sua conta.')
  }
}

export function getMusic(token: string) {
  return request<Music[]>('/api/musicas', {}, token)
}

export function getGenres(token: string) {
  return request<Genre[]>('/api/generos', {}, token)
}

export function getEmotions(token: string) {
  return request<Emotion[]>('/api/emocoes', {}, token)
}

export async function createMusic(token: string, music: Omit<Music, 'id' | 'genero' | 'emocao'>) {
  const result = await request<unknown>('/api/musicas', {
    method: 'POST',
    body: JSON.stringify(music),
  }, token)
  assertSuccessfulMessage(result)
}

export async function updateMusic(token: string, id: number, music: Omit<Music, 'id' | 'genero' | 'emocao'>) {
  const result = await request<unknown>(`/api/musicas/${id}`, {
    method: 'PUT',
    body: JSON.stringify(music),
  }, token)
  assertSuccessfulMessage(result)
}

export async function deleteMusic(token: string, id: number) {
  const result = await request<unknown>(`/api/musicas/${id}`, { method: 'DELETE' }, token)
  assertSuccessfulMessage(result)
}
