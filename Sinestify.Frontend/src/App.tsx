import { useEffect, useMemo, useState, type FormEvent } from 'react'
import {
  ApiError,
  authenticate,
  createMusic,
  deleteMusic,
  getEmotions,
  getGenres,
  getMusic,
  registerUser,
  updateMusic,
  type Emotion,
  type Genre,
  type Music,
} from './api'
import './App.css'

type MusicForm = Omit<Music, 'id' | 'genero' | 'emocao'>

const emptyMusic: MusicForm = {
  nome: '',
  cantor: '',
  generoId: 0,
  emocaoId: 0,
  velocidade: 90,
}

function Icon({
  name,
  size = 20,
}: {
  name: 'music' | 'search' | 'plus' | 'play' | 'pause' | 'edit' | 'trash' | 'close' | 'logout' | 'sparkle'
  size?: number
}) {
  const paths = {
    music: <><path d="M9 18V5l12-2v13" /><circle cx="6" cy="18" r="3" /><circle cx="18" cy="16" r="3" /></>,
    search: <><circle cx="11" cy="11" r="7" /><path d="m20 20-4-4" /></>,
    plus: <><path d="M12 5v14M5 12h14" /></>,
    play: <path d="m8 5 12 7-12 7z" />,
    pause: <><path d="M8 5h3v14H8zM15 5h3v14h-3z" /></>,
    edit: <><path d="m15 5 4 4M4 20l4-.8L19 8a2.1 2.1 0 0 0-3-3L5 16z" /></>,
    trash: <><path d="M4 7h16M10 11v6M14 11v6M6 7l1 14h10l1-14M9 7V4h6v3" /></>,
    close: <><path d="m18 6-12 12M6 6l12 12" /></>,
    logout: <><path d="M10 17l5-5-5-5M15 12H3M21 19V5a2 2 0 0 0-2-2h-6" /></>,
    sparkle: <><path d="m12 3 1.8 5.2L19 10l-5.2 1.8L12 17l-1.8-5.2L5 10l5.2-1.8z" /><path d="m19 14 .9 2.1L22 17l-2.1.9L19 20l-.9-2.1L16 17l2.1-.9z" /></>,
  }

  return (
    <svg
      aria-hidden="true"
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeLinecap="round"
      strokeLinejoin="round"
      strokeWidth="1.7"
    >
      {paths[name]}
    </svg>
  )
}

function getErrorMessage(error: unknown) {
  return error instanceof Error ? error.message : 'Ocorreu um erro inesperado.'
}

function App() {
  const [token, setToken] = useState(() => sessionStorage.getItem('sinestify-token'))
  const [userName, setUserName] = useState(() => sessionStorage.getItem('sinestify-name') ?? '')
  const [isRegistering, setIsRegistering] = useState(false)
  const [authError, setAuthError] = useState('')
  const [authLoading, setAuthLoading] = useState(false)
  const [libraryLoading, setLibraryLoading] = useState(() => Boolean(sessionStorage.getItem('sinestify-token')))
  const [music, setMusic] = useState<Music[]>([])
  const [genres, setGenres] = useState<Genre[]>([])
  const [emotions, setEmotions] = useState<Emotion[]>([])
  const [query, setQuery] = useState('')
  const [genreFilter, setGenreFilter] = useState('all')
  const [emotionFilter, setEmotionFilter] = useState('all')
  const [modalOpen, setModalOpen] = useState(false)
  const [editingMusic, setEditingMusic] = useState<Music | null>(null)
  const [form, setForm] = useState<MusicForm>(emptyMusic)
  const [formError, setFormError] = useState('')
  const [saving, setSaving] = useState(false)
  const [notice, setNotice] = useState('')
  const [pageError, setPageError] = useState('')
  const [playingMusicId, setPlayingMusicId] = useState<number | null>(null)

  useEffect(() => {
    if (!token) return

    let active = true

    Promise.all([getMusic(token), getGenres(token), getEmotions(token)])
      .then(([musicList, genreList, emotionList]) => {
        if (!active) return
        setMusic(musicList)
        setGenres(genreList)
        setEmotions(emotionList)
      })
      .catch((error: unknown) => {
        if (!active) return
        if (error instanceof ApiError && error.status === 401) {
          sessionStorage.removeItem('sinestify-token')
          sessionStorage.removeItem('sinestify-name')
          setToken(null)
          setAuthError('Sua sessão expirou. Entre novamente para continuar.')
          return
        }
        setPageError(getErrorMessage(error))
      })
      .finally(() => {
        if (active) setLibraryLoading(false)
      })

    return () => {
      active = false
    }
  }, [token])

  const filteredMusic = useMemo(() => {
    const normalizedQuery = query.trim().toLocaleLowerCase('pt-BR')
    return music.filter((item) => {
      const matchesQuery =
        !normalizedQuery ||
        item.nome.toLocaleLowerCase('pt-BR').includes(normalizedQuery) ||
        item.cantor.toLocaleLowerCase('pt-BR').includes(normalizedQuery)
      const matchesGenre = genreFilter === 'all' || item.generoId === Number(genreFilter)
      const matchesEmotion = emotionFilter === 'all' || item.emocaoId === Number(emotionFilter)
      return matchesQuery && matchesGenre && matchesEmotion
    })
  }, [emotionFilter, genreFilter, music, query])

  async function handleAuthentication(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setAuthError('')
    setPageError('')
    setAuthLoading(true)

    const data = new FormData(event.currentTarget)
    const email = String(data.get('email') ?? '').trim()
    const senha = String(data.get('senha') ?? '')

    try {
      if (isRegistering) {
        const nome = String(data.get('nome') ?? '').trim()
        await registerUser({ nome, email, senha })
      }

      const result = await authenticate({ email, senha })
      sessionStorage.setItem('sinestify-token', result.token)
      sessionStorage.setItem('sinestify-name', result.name)
      setUserName(result.name)
      setLibraryLoading(true)
      setToken(result.token)
    } catch (error) {
      setAuthError(getErrorMessage(error))
    } finally {
      setAuthLoading(false)
    }
  }

  function signOut() {
    sessionStorage.removeItem('sinestify-token')
    sessionStorage.removeItem('sinestify-name')
    setToken(null)
    setLibraryLoading(false)
    setMusic([])
    setGenres([])
    setEmotions([])
    setPlayingMusicId(null)
    setPageError('')
  }

  function openCreateModal() {
    setEditingMusic(null)
    setForm({ ...emptyMusic, generoId: genres[0]?.id ?? 0, emocaoId: emotions[0]?.id ?? 0 })
    setFormError('')
    setModalOpen(true)
  }

  function openEditModal(item: Music) {
    setEditingMusic(item)
    setForm({
      nome: item.nome,
      cantor: item.cantor,
      generoId: item.generoId,
      emocaoId: item.emocaoId,
      velocidade: item.velocidade,
    })
    setFormError('')
    setModalOpen(true)
  }

  async function refreshMusic() {
    if (!token) return
    setMusic(await getMusic(token))
  }

  async function handleSaveMusic(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!token) return

    setSaving(true)
    setFormError('')

    try {
      if (editingMusic) {
        await updateMusic(token, editingMusic.id, form)
        setNotice('Música atualizada com sucesso.')
      } else {
        await createMusic(token, form)
        setNotice('Música adicionada à sua biblioteca.')
      }
      await refreshMusic()
      setModalOpen(false)
    } catch (error) {
      setFormError(getErrorMessage(error))
    } finally {
      setSaving(false)
    }
  }

  async function handleDeleteMusic(item: Music) {
    if (!token || !window.confirm(`Remover “${item.nome}” da sua biblioteca?`)) return

    setPageError('')
    try {
      await deleteMusic(token, item.id)
      if (playingMusicId === item.id) setPlayingMusicId(null)
      setMusic((current) => current.filter((entry) => entry.id !== item.id))
      setNotice('Música removida da biblioteca.')
    } catch (error) {
      setPageError(getErrorMessage(error))
    }
  }

  if (!token) {
    return (
      <main className="auth-page">
        <section className="auth-art" aria-label="Sinestify">
          <a className="brand brand-light" href="/" aria-label="Sinestify início">
            <span className="brand-mark"><Icon name="music" size={22} /></span>
            <span>sinestify</span>
          </a>
          <div className="auth-art-copy">
            <span className="eyebrow"><Icon name="sparkle" size={15} /> MÚSICA E SENTIMENTO</span>
            <h1>Encontre a trilha do seu momento.</h1>
            <p>Organize músicas por gênero, emoção e ritmo. Sua biblioteca, do seu jeito.</p>
            <div className="art-equalizer" aria-hidden="true">
              {[36, 58, 86, 48, 72, 100, 62, 42, 78, 54, 92, 40, 68, 34, 60, 84, 48, 70, 38, 56, 90, 44, 66, 34].map((height, index) => (
                <span key={index} style={{ height: `${height}%`, animationDelay: `${index * 45}ms` }} />
              ))}
            </div>
          </div>
          <span className="auth-art-footer">Sua música acompanha você.</span>
        </section>

        <section className="auth-panel">
          <div className="auth-form-wrap">
            <span className="auth-kicker">BEM-VINDO AO SINESTIFY</span>
            <h2>{isRegistering ? 'Crie sua conta' : 'Entre na sua conta'}</h2>
            <p className="auth-subtitle">
              {isRegistering ? 'Comece a montar uma biblioteca que combina com você.' : 'Sua biblioteca musical está esperando por você.'}
            </p>

            <form className="auth-form" onSubmit={handleAuthentication}>
              {isRegistering && (
                <label>
                  Seu nome
                  <input name="nome" type="text" autoComplete="name" maxLength={100} placeholder="Como podemos chamar você?" required />
                </label>
              )}
              <label>
                E-mail
                <input name="email" type="email" autoComplete="email" maxLength={254} placeholder="voce@email.com" required />
              </label>
              <label>
                Senha
                <input name="senha" type="password" autoComplete={isRegistering ? 'new-password' : 'current-password'} minLength={isRegistering ? 8 : undefined} maxLength={128} placeholder="Mínimo de 8 caracteres" required />
              </label>
              {authError && <p className="form-message error-message" role="alert">{authError}</p>}
              <button className="button button-primary auth-submit" type="submit" disabled={authLoading}>
                {authLoading ? 'Aguarde...' : isRegistering ? 'Criar conta' : 'Entrar'}
              </button>
            </form>

            <p className="auth-switch">
              {isRegistering ? 'Já tem uma conta?' : 'Ainda não tem uma conta?'}{' '}
              <button type="button" onClick={() => { setIsRegistering((current) => !current); setAuthError('') }}>
                {isRegistering ? 'Fazer login' : 'Cadastre-se'}
              </button>
            </p>
          </div>
          <span className="auth-legal">Feito para ouvir com outros sentidos.</span>
        </section>
      </main>
    )
  }

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <a className="brand" href="/" aria-label="Sinestify início">
          <span className="brand-mark"><Icon name="music" size={22} /></span>
          <span>sinestify</span>
        </a>

        <div className="sidebar-section">
          <span className="sidebar-label">MENU</span>
          <button type="button" className="nav-link active">
            <Icon name="music" size={18} />
            <span>Minha biblioteca</span>
          </button>
        </div>

        <div className="sidebar-card">
          <div className="sidebar-card-icon"><Icon name="sparkle" size={18} /></div>
          <strong>Uma música para cada sentimento.</strong>
          <p>Filtre sua coleção e encontre o som que combina com agora.</p>
        </div>

        <button type="button" className="nav-link signout-link" onClick={signOut}>
          <Icon name="logout" size={18} />
          <span>Sair da conta</span>
        </button>
      </aside>

      <main className="main-content">
        <header className="topbar">
          <div className="breadcrumb">Sua biblioteca <span>/</span> <strong>Visão geral</strong></div>
          <div className="user-chip">
            <span className="user-avatar">{userName ? userName.charAt(0).toUpperCase() : 'S'}</span>
            <span>{userName || 'Sua conta'}</span>
          </div>
        </header>

        <div className="content-wrap">
          <section className="welcome-row">
            <div>
              <span className="eyebrow"><Icon name="sparkle" size={15} /> SEU ESPAÇO MUSICAL</span>
              <h1>O que combina com seu momento?</h1>
              <p>Explore sua coleção e deixe a música acompanhar o seu ritmo.</p>
            </div>
            <button type="button" className="button button-primary add-button" onClick={openCreateModal}>
              <Icon name="plus" size={19} />
              <span>Adicionar música</span>
            </button>
          </section>

          <section className="hero-card">
            <div className="hero-copy">
              <span className="hero-label">SUA COLEÇÃO, EM SINTONIA</span>
              <h2>Uma biblioteca<br />cheia de significado.</h2>
              <p>Encontre novas conexões entre suas músicas favoritas, seus sentimentos e seu ritmo.</p>
              <span className="hero-count"><strong>{music.length.toString().padStart(2, '0')}</strong> {music.length === 1 ? 'música na coleção' : 'músicas na coleção'}</span>
            </div>
            <div className="hero-visual" aria-hidden="true">
              <div className="vinyl"><span /><i /></div>
              <div className="hero-note note-one">♪</div>
              <div className="hero-note note-two">♫</div>
              <div className="hero-orbit orbit-one" />
              <div className="hero-orbit orbit-two" />
            </div>
          </section>

          <section className="library-section">
            <div className="section-heading">
              <div>
                <span className="section-overline">DESCUBRA SUA TRILHA</span>
                <h2>Minha biblioteca <span>{music.length}</span></h2>
              </div>
              <span className="library-note">Classificada por adição</span>
            </div>

            <div className="filter-bar">
              <label className="search-field">
                <Icon name="search" size={18} />
                <input value={query} onChange={(event) => setQuery(event.target.value)} placeholder="Buscar por música ou artista" aria-label="Buscar por música ou artista" />
              </label>
              <label className="filter-select">
                <span>Gênero</span>
                <select value={genreFilter} onChange={(event) => setGenreFilter(event.target.value)}>
                  <option value="all">Todos</option>
                  {genres.map((genre) => <option key={genre.id} value={genre.id}>{genre.nome}</option>)}
                </select>
              </label>
              <label className="filter-select">
                <span>Emoção</span>
                <select value={emotionFilter} onChange={(event) => setEmotionFilter(event.target.value)}>
                  <option value="all">Todas</option>
                  {emotions.map((emotion) => <option key={emotion.id} value={emotion.id}>{emotion.sentimento}</option>)}
                </select>
              </label>
            </div>

            {notice && <div className="notice" role="status">{notice}<button type="button" aria-label="Fechar aviso" onClick={() => setNotice('')}><Icon name="close" size={16} /></button></div>}
            {pageError && <div className="form-message error-message" role="alert">{pageError}</div>}

            {libraryLoading ? (
              <div className="empty-state"><span className="loader" />Carregando sua biblioteca...</div>
            ) : filteredMusic.length > 0 ? (
              <div className="music-grid">
                {filteredMusic.map((item, index) => {
                  const isPlaying = playingMusicId === item.id
                  return (
                    <article className={`music-card${isPlaying ? ' is-playing' : ''}`} key={item.id}>
                      <div className={`album-art album-art-${(index % 5) + 1}`}>
                        <div className="album-art-orbit" />
                        <Icon name="music" size={30} />
                        <span className="album-art-index">{String(index + 1).padStart(2, '0')}</span>
                        <button
                          type="button"
                          className="play-button"
                          aria-label={isPlaying ? `Parar visualização de ${item.nome}` : `Visualizar ritmo de ${item.nome}`}
                          onClick={() => setPlayingMusicId(isPlaying ? null : item.id)}
                        >
                          <Icon name={isPlaying ? 'pause' : 'play'} size={18} />
                        </button>
                      </div>
                      <div className="music-card-content">
                        <div className="music-title-line">
                          <div>
                            <h3 title={item.nome}>{item.nome}</h3>
                            <p title={item.cantor}>{item.cantor}</p>
                          </div>
                          <div className="card-actions">
                            <button type="button" aria-label={`Editar ${item.nome}`} onClick={() => openEditModal(item)}><Icon name="edit" size={17} /></button>
                            <button type="button" aria-label={`Excluir ${item.nome}`} onClick={() => void handleDeleteMusic(item)}><Icon name="trash" size={17} /></button>
                          </div>
                        </div>
                        <div className="music-meta">
                          <span>{item.genero?.nome ?? genres.find((genre) => genre.id === item.generoId)?.nome ?? 'Sem gênero'}</span>
                          <span className="mood-tag">{item.emocao?.sentimento ?? emotions.find((emotion) => emotion.id === item.emocaoId)?.sentimento ?? 'Sem emoção'}</span>
                        </div>
                        <div className="tempo-row">
                          <span className={`mini-equalizer${isPlaying ? ' animate' : ''}`} aria-hidden="true"><i /><i /><i /><i /></span>
                          <span>{item.velocidade} BPM</span>
                          {isPlaying && <span className="playing-label">ritmo ativo</span>}
                        </div>
                      </div>
                    </article>
                  )
                })}
              </div>
            ) : (
              <div className="empty-state">
                <span className="empty-icon"><Icon name="music" size={26} /></span>
                <h3>{music.length === 0 ? 'Sua biblioteca começa aqui' : 'Nenhuma música encontrada'}</h3>
                <p>{music.length === 0 ? 'Adicione uma música para criar sua coleção.' : 'Tente ajustar a busca ou os filtros selecionados.'}</p>
                {music.length === 0 && <button type="button" className="button button-primary" onClick={openCreateModal}><Icon name="plus" size={18} />Adicionar música</button>}
              </div>
            )}
          </section>

          <footer className="page-footer">
            <span>Sinestify <span className="footer-dot">•</span> Música, emoção e movimento.</span>
            <span>Feito para ouvir com outros sentidos.</span>
          </footer>
        </div>
      </main>

      {playingMusicId !== null && (
        <aside className="rhythm-player" aria-live="polite">
          <span className="player-note"><Icon name="music" size={18} /></span>
          <div className="player-track">
            <strong>{music.find((item) => item.id === playingMusicId)?.nome}</strong>
            <span>Visualização rítmica · sem reprodução de áudio</span>
          </div>
          <div className="player-equalizer" aria-hidden="true">
            {Array.from({ length: 28 }, (_, index) => <i key={index} style={{ animationDelay: `${index * 32}ms` }} />)}
          </div>
          <button type="button" className="player-stop" onClick={() => setPlayingMusicId(null)} aria-label="Parar visualização"><Icon name="pause" size={20} /></button>
        </aside>
      )}

      {modalOpen && (
        <div className="modal-backdrop" role="presentation" onMouseDown={(event) => { if (event.target === event.currentTarget) setModalOpen(false) }}>
          <section className="music-modal" role="dialog" aria-modal="true" aria-labelledby="modal-title">
            <div className="modal-heading">
              <div>
                <span className="section-overline">{editingMusic ? 'ATUALIZE SUA COLEÇÃO' : 'ADICIONE À SUA COLEÇÃO'}</span>
                <h2 id="modal-title">{editingMusic ? 'Editar música' : 'Nova música'}</h2>
              </div>
              <button type="button" className="icon-button" aria-label="Fechar" onClick={() => setModalOpen(false)}><Icon name="close" /></button>
            </div>
            <form className="music-form" onSubmit={handleSaveMusic}>
              <label>
                Nome da música
                <input value={form.nome} onChange={(event) => setForm((current) => ({ ...current, nome: event.target.value }))} maxLength={200} required autoFocus placeholder="Ex.: Minha canção favorita" />
              </label>
              <label>
                Artista
                <input value={form.cantor} onChange={(event) => setForm((current) => ({ ...current, cantor: event.target.value }))} maxLength={200} required placeholder="Quem interpreta?" />
              </label>
              <div className="form-row">
                <label>
                  Gênero
                  <select value={form.generoId || ''} onChange={(event) => setForm((current) => ({ ...current, generoId: Number(event.target.value) }))} required>
                    <option value="" disabled>Selecione</option>
                    {genres.map((genre) => <option key={genre.id} value={genre.id}>{genre.nome}</option>)}
                  </select>
                </label>
                <label>
                  Emoção
                  <select value={form.emocaoId || ''} onChange={(event) => setForm((current) => ({ ...current, emocaoId: Number(event.target.value) }))} required>
                    <option value="" disabled>Selecione</option>
                    {emotions.map((emotion) => <option key={emotion.id} value={emotion.id}>{emotion.sentimento}</option>)}
                  </select>
                </label>
              </div>
              <label>
                Ritmo <span className="label-hint">batidas por minuto</span>
                <input type="number" min={1} step={1} value={form.velocidade} onChange={(event) => setForm((current) => ({ ...current, velocidade: Number(event.target.value) }))} required />
              </label>
              {formError && <p className="form-message error-message" role="alert">{formError}</p>}
              <div className="modal-actions">
                <button type="button" className="button button-quiet" onClick={() => setModalOpen(false)}>Cancelar</button>
                <button type="submit" className="button button-primary" disabled={saving || genres.length === 0 || emotions.length === 0}>
                  {saving ? 'Salvando...' : editingMusic ? 'Salvar alterações' : 'Adicionar música'}
                </button>
              </div>
            </form>
          </section>
        </div>
      )}
    </div>
  )
}

export default App
