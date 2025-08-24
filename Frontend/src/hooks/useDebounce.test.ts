import { describe, it, expect } from 'vitest'
import { renderHook, act } from '@testing-library/react'
import { useDebounce } from '../hooks/useDebounce'

describe('useDebounce', () => {
  it('should return the initial value immediately', () => {
    const { result } = renderHook(() => useDebounce('initial', 500))
    expect(result.current).toBe('initial')
  })

  it('should debounce the value change', async () => {
    const { result, rerender } = renderHook(
      ({ value, delay }) => useDebounce(value, delay),
      {
        initialProps: { value: 'initial', delay: 100 }
      }
    )

    expect(result.current).toBe('initial')

    // Change the value
    rerender({ value: 'updated', delay: 100 })
    
    // Should still be the initial value immediately
    expect(result.current).toBe('initial')

    // Wait for debounce delay
    await act(async () => {
      await new Promise(resolve => setTimeout(resolve, 150))
    })

    // Now should be updated
    expect(result.current).toBe('updated')
  })

  it('should use default delay when not provided', async () => {
    const { result, rerender } = renderHook(
      ({ value }) => useDebounce(value),
      {
        initialProps: { value: 'initial' }
      }
    )

    rerender({ value: 'updated' })
    expect(result.current).toBe('initial')

    // Wait for default delay (300ms)
    await act(async () => {
      await new Promise(resolve => setTimeout(resolve, 350))
    })

    expect(result.current).toBe('updated')
  })

  it('should cancel previous timeout when value changes quickly', async () => {
    const { result, rerender } = renderHook(
      ({ value }) => useDebounce(value, 100),
      {
        initialProps: { value: 'initial' }
      }
    )

    // Change value multiple times quickly
    rerender({ value: 'first' })
    await act(async () => {
      await new Promise(resolve => setTimeout(resolve, 50))
    })
    
    rerender({ value: 'second' })
    await act(async () => {
      await new Promise(resolve => setTimeout(resolve, 50))
    })
    
    rerender({ value: 'final' })

    // Should still be initial
    expect(result.current).toBe('initial')

    // Wait for full delay
    await act(async () => {
      await new Promise(resolve => setTimeout(resolve, 150))
    })

    // Should be the final value, not intermediate ones
    expect(result.current).toBe('final')
  })

  it('should handle different types of values', async () => {
    const { result, rerender } = renderHook(
      ({ value }: { value: number }) => useDebounce(value, 50),
      {
        initialProps: { value: 1 }
      }
    )

    expect(result.current).toBe(1)

    rerender({ value: 42 })
    
    await act(async () => {
      await new Promise(resolve => setTimeout(resolve, 100))
    })

    expect(result.current).toBe(42)
  })
})
