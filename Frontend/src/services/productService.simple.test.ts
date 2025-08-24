import { describe, it, expect, vi, beforeEach } from 'vitest'
import { productService } from '../services/productService'
import { mockProducts } from '../test/test-utils'

// Mock the entire API client module
vi.mock('../services/api', () => ({
  default: {
    get: vi.fn(),
  }
}))

import apiClient from '../services/api'
const mockGet = vi.mocked(apiClient.get)

describe('productService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('getAllProducts', () => {
    it('should return all products', async () => {
      mockGet.mockResolvedValueOnce({ data: mockProducts } as any)

      const result = await productService.getAllProducts()

      expect(mockGet).toHaveBeenCalledWith('/api/Product')
      expect(result).toEqual(mockProducts)
    })
  })

  describe('searchProducts', () => {
    it('should return search results', async () => {
      const searchTerm = 'test'
      const searchResults = [mockProducts[0]]
      mockGet.mockResolvedValueOnce({ data: searchResults } as any)

      const result = await productService.searchProducts(searchTerm)

      expect(mockGet).toHaveBeenCalledWith('/api/Product/search?searchTerm=test')
      expect(result).toEqual(searchResults)
    })

    it('should return empty array for empty search term', async () => {
      const result = await productService.searchProducts('')

      expect(result).toEqual([])
      expect(mockGet).not.toHaveBeenCalled()
    })
  })
})
