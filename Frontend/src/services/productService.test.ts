import { describe, it, expect, vi, beforeEach } from 'vitest'
import { productService } from '../services/productService'
import { mockProducts } from '../test/test-utils'

// Mock the entire API client module
vi.mock('../services/api', () => ({
  default: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
    delete: vi.fn(),
  }
}))

// Import the mocked API client
import apiClient from '../services/api'
const mockedGet = vi.mocked(apiClient.get)

describe('productService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('getAllProducts', () => {
    it('should return all products', async () => {
      mockedApiClient.get.mockResolvedValueOnce({ data: mockProducts })

      const result = await productService.getAllProducts()

      expect(mockedApiClient.get).toHaveBeenCalledWith('/api/Product')
      expect(result).toEqual(mockProducts)
    })

    it('should throw error when API fails', async () => {
      const errorMessage = 'API Error'
      mockedApiClient.get.mockRejectedValueOnce(new Error(errorMessage))

      await expect(productService.getAllProducts()).rejects.toThrow(errorMessage)
      expect(mockedApiClient.get).toHaveBeenCalledWith('/api/Product')
    })
  })

  describe('searchProducts', () => {
    it('should return search results', async () => {
      const searchTerm = 'test'
      const searchResults = [mockProducts[0]]
      mockedApiClient.get.mockResolvedValueOnce({ data: searchResults })

      const result = await productService.searchProducts(searchTerm)

      expect(mockedApiClient.get).toHaveBeenCalledWith('/api/Product/search?searchTerm=test')
      expect(result).toEqual(searchResults)
    })

    it('should encode search term in URL', async () => {
      const searchTerm = 'test & special'
      mockedApiClient.get.mockResolvedValueOnce({ data: [] })

      await productService.searchProducts(searchTerm)

      expect(mockedApiClient.get).toHaveBeenCalledWith('/api/Product/search?searchTerm=test%20%26%20special')
    })

    it('should return empty array for empty search term', async () => {
      const result = await productService.searchProducts('')

      expect(result).toEqual([])
      expect(mockedApiClient.get).not.toHaveBeenCalled()
    })

    it('should return empty array for whitespace-only search term', async () => {
      const result = await productService.searchProducts('   ')

      expect(result).toEqual([])
      expect(mockedApiClient.get).not.toHaveBeenCalled()
    })

    it('should throw error when API fails', async () => {
      const errorMessage = 'Search API Error'
      mockedApiClient.get.mockRejectedValueOnce(new Error(errorMessage))

      await expect(productService.searchProducts('test')).rejects.toThrow(errorMessage)
    })
  })

  describe('getProductById', () => {
    it('should return product by ID', async () => {
      const productId = '1'
      const product = mockProducts[0]
      mockedApiClient.get.mockResolvedValueOnce({ data: product })

      const result = await productService.getProductById(productId)

      expect(mockedApiClient.get).toHaveBeenCalledWith('/api/Product/1')
      expect(result).toEqual(product)
    })

    it('should throw error when product not found', async () => {
      const productId = 'nonexistent'
      mockedApiClient.get.mockRejectedValueOnce(new Error('Product not found'))

      await expect(productService.getProductById(productId)).rejects.toThrow('Product not found')
    })
  })

  describe('getProductBySku', () => {
    it('should return product by SKU', async () => {
      const sku = 'TEST-001'
      const product = mockProducts[0]
      mockedApiClient.get.mockResolvedValueOnce({ data: product })

      const result = await productService.getProductBySku(sku)

      expect(mockedApiClient.get).toHaveBeenCalledWith('/api/Product/by-sku/TEST-001')
      expect(result).toEqual(product)
    })

    it('should throw error when product with SKU not found', async () => {
      const sku = 'NONEXISTENT'
      mockedApiClient.get.mockRejectedValueOnce(new Error('Product not found'))

      await expect(productService.getProductBySku(sku)).rejects.toThrow('Product not found')
    })
  })
})
