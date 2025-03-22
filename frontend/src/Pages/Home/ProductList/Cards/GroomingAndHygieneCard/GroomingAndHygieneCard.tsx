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
import React from 'react';
import styled from 'styled-components';
import { AnimatePresence, motion } from 'framer-motion';

interface GroomingCardProps {
  product: components['schemas']['GetGroomingAndHygieneResponse'];
}

export const GroomingAndHygieneCard = ({ product }: GroomingCardProps) => {
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
                <p>{product.isCrueltyFree && <strong>Is Cruelty Free</strong>}</p>
                <p>{product.isHypoAllergenic && <strong>Is Hypoallergenic</strong>}</p>
                <p>{product.isNatural && <strong>Made from natural ingredients</strong>}</p>
              </ProductSpecs>
              <WarningSection>
                <SectionTitle>Safety Warnings:</SectionTitle>
                <p>{product.safetyWarnings}</p>
              </WarningSection>
              <UsageSection>
                <SectionTitle>Usage Instructions:</SectionTitle>
                <p>{product.usageInstructions}</p>
              </UsageSection>
            </motion.div>
          )}
        </AnimatePresence>
      </CardContent>

      <CardActions>
        <CardButton variant="contained">Add to cart</CardButton>
        <CardButton onClick={() => setIsExpanded(!isExpanded)} variant="outlined">
          {isExpanded ? 'Hide Details' : 'View Details'}
        </CardButton>
      </CardActions>
    </Card>
  );
};

const WarningSection = styled.div`
  margin-top: 0.75rem;
`;

const UsageSection = styled.div`
  margin-top: 0.75rem;
`;
