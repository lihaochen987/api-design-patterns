import { useState } from 'react';
import { components } from '../../../Shared/types';
import { fetchClient } from '../../../Shared/fetch-client.ts';
import { PetFoodCard } from './Cards/PetFoodCard/PetFoodCard.tsx';
import { GroomingAndHygieneCard } from './Cards/GroomingAndHygieneCard/GroomingAndHygieneCard.tsx';
import { DefaultProductCard } from './Cards/DefaultProductCard/DefaultProductCard.tsx';
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
  const [page, setPage] = useState(1);
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
      <div className={'l-loading-spinner'}>
        <div>
          <div className={'loading-spinner'} />
        </div>
      </div>
    );
  }

  const totalProducts = data?.totalCount || 0;
  const totalPages = Math.ceil(totalProducts / ITEMS_PER_PAGE);

  const products: Array<Product> = data?.results || [];

  return (
    <>
      <div className={'l-product-list'}>
        {products.length === 0 ? (
          <div className={'no-products-message'}>No products found</div>
        ) : (
          products.map(product => (
            <div key={product.id} className="product-item">
              {renderProduct(product)}
            </div>
          ))
        )}
      </div>

      {totalPages > 1 && (
        <div className={'l-pagination'}>
          <Pagination currentPage={page} totalPages={totalPages} onPageChange={handlePageChange} />
        </div>
      )}
    </>
  );
};
