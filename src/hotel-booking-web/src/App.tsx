import { useCallback, useEffect, useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import './App.css'

type Room = {
  id: number
  number: string
  name: string
  description: string
  capacity: number
  pricePerNight: number
  isAvailable: boolean
}

type Booking = {
  id: number
  roomNumber: string
  roomName: string
  guestName: string
  guestEmail: string
  checkIn: string
  checkOut: string
  totalPrice: number
}

type ApiError = {
  message?: string
  title?: string
  errors?: Record<string, string[]>
}

const toDateInput = (date: Date) => date.toISOString().slice(0, 10)
const addDays = (date: Date, days: number) => {
  const next = new Date(date)
  next.setDate(next.getDate() + days)
  return next
}

const today = new Date()

function App() {
  const [checkIn, setCheckIn] = useState(toDateInput(addDays(today, 1)))
  const [checkOut, setCheckOut] = useState(toDateInput(addDays(today, 3)))
  const [rooms, setRooms] = useState<Room[]>([])
  const [bookings, setBookings] = useState<Booking[]>([])
  const [selectedRoom, setSelectedRoom] = useState<Room | null>(null)
  const [guestName, setGuestName] = useState('')
  const [guestEmail, setGuestEmail] = useState('')
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState('')
  const [notice, setNotice] = useState('')

  const nights = useMemo(() => {
    const duration = new Date(checkOut).getTime() - new Date(checkIn).getTime()
    return Math.max(0, Math.round(duration / 86_400_000))
  }, [checkIn, checkOut])

  const loadData = useCallback(async () => {
    setLoading(true)
    setError('')

    try {
      const [roomsResponse, bookingsResponse] = await Promise.all([
        fetch(`/api/rooms?checkIn=${checkIn}&checkOut=${checkOut}`),
        fetch('/api/bookings'),
      ])

      if (!roomsResponse.ok || !bookingsResponse.ok) {
        throw new Error('The hotel service is unavailable.')
      }

      setRooms(await roomsResponse.json())
      setBookings(await bookingsResponse.json())
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : 'Unable to load hotel data.')
    } finally {
      setLoading(false)
    }
  }, [checkIn, checkOut])

  useEffect(() => {
    void loadData()
  }, [loadData])

  const submitBooking = async (event: FormEvent) => {
    event.preventDefault()
    if (!selectedRoom) return

    setSaving(true)
    setError('')
    setNotice('')

    try {
      const response = await fetch('/api/bookings', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          roomId: selectedRoom.id,
          guestName,
          guestEmail,
          checkIn,
          checkOut,
        }),
      })

      if (!response.ok) {
        const problem: ApiError = await response.json()
        const validationMessage = problem.errors
          ? Object.values(problem.errors).flat().join(' ')
          : undefined
        throw new Error(problem.message ?? validationMessage ?? problem.title ?? 'Booking failed.')
      }

      setNotice(`${selectedRoom.name} is booked for ${guestName}.`)
      setSelectedRoom(null)
      setGuestName('')
      setGuestEmail('')
      await loadData()
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : 'Booking failed.')
    } finally {
      setSaving(false)
    }
  }

  const cancelBooking = async (booking: Booking) => {
    setError('')
    setNotice('')

    const response = await fetch(`/api/bookings/${booking.id}`, { method: 'DELETE' })
    if (!response.ok) {
      setError('The booking could not be cancelled.')
      return
    }

    setNotice(`Booking for ${booking.guestName} was cancelled.`)
    await loadData()
  }

  return (
    <main>
      <header className="hero">
        <nav>
          <a className="brand" href="#top">Northstar Hotel</a>
          <a href="#bookings">Bookings</a>
        </nav>
        <div id="top" className="hero-copy">
          <p className="eyebrow">Stay somewhere memorable</p>
          <h1>Find your room by the harbour.</h1>
          <p>Six thoughtful rooms, simple local booking, and no hidden fees.</p>
        </div>
      </header>

      <section className="search-card" aria-label="Search rooms">
        <label>
          Check in
          <input
            type="date"
            min={toDateInput(today)}
            value={checkIn}
            onChange={(event) => setCheckIn(event.target.value)}
          />
        </label>
        <label>
          Check out
          <input
            type="date"
            min={checkIn}
            value={checkOut}
            onChange={(event) => setCheckOut(event.target.value)}
          />
        </label>
        <div className="stay-summary">
          <span>Length of stay</span>
          <strong>{nights} {nights === 1 ? 'night' : 'nights'}</strong>
        </div>
      </section>

      <section className="content-section">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Available stays</p>
            <h2>Choose your room</h2>
          </div>
          <span>{rooms.filter((room) => room.isAvailable).length} rooms available</span>
        </div>

        {error && <div className="alert error" role="alert">{error}</div>}
        {notice && <div className="alert success" role="status">{notice}</div>}

        {loading ? (
          <p className="empty-state">Checking availability...</p>
        ) : (
          <div className="room-grid">
            {rooms.map((room) => (
              <article className={`room-card ${room.isAvailable ? '' : 'unavailable'}`} key={room.id}>
                <div className={`room-image room-${room.id}`}>
                  <span>Room {room.number}</span>
                </div>
                <div className="room-body">
                  <div className="room-title">
                    <div>
                      <h3>{room.name}</h3>
                      <span>Up to {room.capacity} guests</span>
                    </div>
                    <p><strong>${room.pricePerNight}</strong> / night</p>
                  </div>
                  <p>{room.description}</p>
                  <button
                    type="button"
                    disabled={!room.isAvailable || nights < 1}
                    onClick={() => setSelectedRoom(room)}
                  >
                    {room.isAvailable ? 'Book this room' : 'Unavailable'}
                  </button>
                </div>
              </article>
            ))}
          </div>
        )}
      </section>

      {selectedRoom && (
        <div className="dialog-backdrop" role="presentation" onMouseDown={() => setSelectedRoom(null)}>
          <section
            className="booking-dialog"
            role="dialog"
            aria-modal="true"
            aria-labelledby="booking-title"
            onMouseDown={(event) => event.stopPropagation()}
          >
            <button className="close-button" type="button" onClick={() => setSelectedRoom(null)}>
              Close
            </button>
            <p className="eyebrow">Complete your stay</p>
            <h2 id="booking-title">{selectedRoom.name}</h2>
            <p>{checkIn} to {checkOut}, {nights} nights</p>
            <form onSubmit={submitBooking}>
              <label>
                Guest name
                <input
                  required
                  maxLength={120}
                  value={guestName}
                  onChange={(event) => setGuestName(event.target.value)}
                />
              </label>
              <label>
                Email
                <input
                  required
                  type="email"
                  value={guestEmail}
                  onChange={(event) => setGuestEmail(event.target.value)}
                />
              </label>
              <div className="price-row">
                <span>Total</span>
                <strong>${selectedRoom.pricePerNight * nights}</strong>
              </div>
              <button type="submit" disabled={saving}>
                {saving ? 'Booking...' : 'Confirm booking'}
              </button>
            </form>
          </section>
        </div>
      )}

      <section id="bookings" className="content-section bookings-section">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Front desk view</p>
            <h2>Current bookings</h2>
          </div>
        </div>
        {bookings.length === 0 ? (
          <p className="empty-state">No bookings yet. Choose a room above to create one.</p>
        ) : (
          <div className="booking-list">
            {bookings.map((booking) => (
              <article key={booking.id}>
                <div>
                  <strong>{booking.roomName}</strong>
                  <span>Room {booking.roomNumber} | {booking.checkIn} to {booking.checkOut}</span>
                </div>
                <div>
                  <strong>{booking.guestName}</strong>
                  <span>{booking.guestEmail}</span>
                </div>
                <strong>${booking.totalPrice}</strong>
                <button type="button" className="secondary" onClick={() => void cancelBooking(booking)}>
                  Cancel
                </button>
              </article>
            ))}
          </div>
        )}
      </section>
    </main>
  )
}

export default App
