import { useState } from 'react';
import { components } from '../../../Shared/types';
import { fetchClient } from '../../../Shared/fetch-client.ts';
import { PetFoodCard } from './Cards/PetFoodCard/PetFoodCard.tsx';
import { GroomingAndHygieneCard } from './Cards/GroomingAndHygieneCard/GroomingAndHygieneCard.tsx';
import { DefaultProductCard } from './Cards/DefaultProductCard/DefaultProductCard.tsx';
import styled from 'styled-components';
import { useQuery } from '@tanstack/react-query';
import { Pagination } from './Pagination/Pagination.tsx';

type Product =
  | components['schemas']['GetProductResponse']
  | components['schemas']['GetPetFoodResponse']
  | components['schemas']['GetGroomingAndHygieneResponse'];

const useProducts = (page: number, pageSize: number) => {
  return useQuery({
    queryKey: ['products', page, pageSize],
    queryFn: async () => {
      const { data, error } = await fetchClient.GET('/products', {
        params: {
          query: {
            PageToken: String(page),
            MaxPageSize: pageSize,
          },
        },
      });

      if (error) throw error;
      return data;
    },
  });
};

export const ProductList = () => {
  const [page, setPage] = useState(1); // Changed to start from 1 for better UX
  const ITEMS_PER_PAGE = 12;
  const { data, isLoading } = useProducts((page - 1) * ITEMS_PER_PAGE, ITEMS_PER_PAGE);

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const renderProduct = (product: Product) => {
    switch (product.category) {
      case 'PetFood':
        return <PetFoodCard product={product as components['schemas']['GetPetFoodResponse']} />;
      case 'GroomingAndHygiene':
        return (
          <GroomingAndHygieneCard
            product={product as components['schemas']['GetGroomingAndHygieneResponse']}
          />
        );
      default:
        return <DefaultProductCard product={product} />;
    }
  };

  if (isLoading) {
    return (
      <LoadingContainer>
        <SpinnerContainer>
          <Spinner />
        </SpinnerContainer>
      </LoadingContainer>
    );
  }

  const totalProducts = data?.totalCount || 0;
  const totalPages = Math.ceil(totalProducts / ITEMS_PER_PAGE);

  const products: Array<Product> = data?.results || [];

  return (
    <>
      <ProductListContainer>
        {products.length === 0 ? (
          <NoProductsMessage>No products found</NoProductsMessage>
        ) : (
          products.map(product => (
            <div key={product.id} className="product-item">
              {renderProduct(product)}
            </div>
          ))
        )}
      </ProductListContainer>

      {totalPages > 1 && (
        <PaginationContainer>
          <Pagination currentPage={page} totalPages={totalPages} onPageChange={handlePageChange} />
        </PaginationContainer>
      )}
    </>
  );
};

const ProductListContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.25rem;
  padding: 1.25rem;
`;

const PaginationContainer = styled.div`
  display: flex;
  justify-content: center;
  padding: 2rem 0;
`;

const LoadingContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  height: 300px;
  width: 100%;
`;

const NoProductsMessage = styled.div`
  grid-column: 1 / -1;
  text-align: center;
  padding: 2rem;
  font-size: 1.2rem;
  color: #666;
`;

const SpinnerContainer = styled.div`
  display: inline-block;
  position: relative;
  width: 40px;
  height: 40px;
`;

const Spinner = styled.div`
  border: 4px solid rgba(0, 0, 0, 0.1);
  border-radius: 50%;
  border-top: 4px solid #1976d2;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;

  @keyframes spin {
    0% {
      transform: rotate(0deg);
    }
    100% {
      transform: rotate(360deg);
    }
  }
`;
