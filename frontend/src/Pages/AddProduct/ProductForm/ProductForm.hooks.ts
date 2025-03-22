import { useMutation, useQueryClient } from '@tanstack/react-query';
import { CreateProductRequest, CreateProductResponse } from './ProductForm.types.ts';
import { fetchClient } from '../../../Shared/fetch-client.ts';

export const useCreateProduct = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (newProduct: CreateProductRequest): Promise<CreateProductResponse> => {
      const { data, error } = await fetchClient.POST('/product', {
        body: newProduct,
      });

      if (error) {
        console.error('API error:', error);
        throw new Error(String(error.message) || 'Failed to create product');
      }

      if (!data) {
        throw new Error('No data returned from the server');
      }

      return data;
    },
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: ['products'] });
    },
  });
};
