import { components } from '../../../../../Shared/types';
import {
  Card,
  CardActions,
  CardButton,
  CardContent,
  CardHeader,
  Price,
  ProductDimensions,
  ProductSpecs,
  SectionTitle,
} from '../../ProductList.styles.ts';
import styled from 'styled-components';
import React from 'react';
import { AnimatePresence, motion } from 'framer-motion';

interface PetFoodCardProps {
  product: components['schemas']['GetPetFoodResponse'];
}

export const PetFoodCard = ({ product }: PetFoodCardProps) => {
  const [isExpanded, setIsExpanded] = React.useState(false);
  return (
    <Card>
      <CardHeader>
        <h1>{product.name}</h1>
        <Price>${product.price}</Price>
      </CardHeader>

      <CardContent>
        <p>
          <strong>Category:</strong> {product.category}
        </p>

        <ProductDimensions>
          <p>
            <strong>Dimensions:</strong> {product.dimensions.length} x {product.dimensions.width} x{' '}
            {product.dimensions.height}
          </p>
        </ProductDimensions>

        <AnimatePresence>
          {isExpanded && (
            <motion.div
              initial={{ opacity: 0, height: 0 }}
              animate={{ opacity: 1, height: 'auto' }}
              exit={{ opacity: 0, height: 0 }}
              transition={{
                opacity: { duration: 0.3, ease: 'easeOut' },
                height: { duration: 0.4, ease: 'easeOut' },
              }}
              style={{ overflow: 'hidden' }}
            >
              <ProductSpecs>
                <p>
                  <strong>Age Group:</strong> {product.ageGroup}
                </p>
                <p>
                  <strong>Breed Size:</strong> {product.breedSize}
                </p>
                <p>
                  <strong>Weight:</strong> {product.weightKg} kg
                </p>
              </ProductSpecs>

              <NutritionSection>
                <SectionTitle>Nutritional Information</SectionTitle>
                <NutritionTable>
                  {Object.entries(product.nutritionalInfo).map(([key, value]) => (
                    <NutritionRow key={key}>
                      <NutritionKey>{key}:</NutritionKey>
                      <NutritionValue>{value}</NutritionValue>
                    </NutritionRow>
                  ))}
                </NutritionTable>
              </NutritionSection>

              <IngredientsSection>
                <SectionTitle>Ingredients</SectionTitle>
                <p>{product.ingredients}</p>
              </IngredientsSection>

              <StorageSection>
                <SectionTitle>Storage Instructions</SectionTitle>
                <p>{product.storageInstructions}</p>
              </StorageSection>
            </motion.div>
          )}
        </AnimatePresence>
      </CardContent>

      <CardActions>
        <CardButton variant="contained">Add to cart</CardButton>
        <CardButton variant="outlined" onClick={() => setIsExpanded(!isExpanded)}>
          View Details
        </CardButton>
      </CardActions>
    </Card>
  );
};

export const NutritionSection = styled.div`
  margin-top: 0.75rem;
`;

export const NutritionTable = styled.div`
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 0.5rem;
  margin-top: 0.5rem;
`;

export const NutritionRow = styled.div`
  display: flex;
  justify-content: space-between;
  padding: 0.25rem;
  background-color: #f9f9f9;
  border-radius: 4px;
`;

export const NutritionKey = styled.span`
  font-weight: 500;
`;

export const NutritionValue = styled.span``;

export const IngredientsSection = styled.div`
  margin-top: 0.75rem;
`;

export const StorageSection = styled.div`
  margin-top: 0.75rem;
`;
