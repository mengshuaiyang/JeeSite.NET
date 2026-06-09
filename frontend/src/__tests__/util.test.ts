import { describe, it, expect } from 'vitest'

describe('utility', () => {
  it('localStorage darkMode round-trip', () => {
    localStorage.setItem('darkMode', 'true')
    expect(localStorage.getItem('darkMode')).toBe('true')
    localStorage.setItem('darkMode', 'false')
    expect(localStorage.getItem('darkMode')).toBe('false')
  })

  it('Date formatting', () => {
    const d = new Date('2026-06-09T10:00:00')
    expect(d.toISOString()).toContain('2026-06-09')
  })
})
