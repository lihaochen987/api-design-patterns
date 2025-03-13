import { components } from '../../../../../Shared/types';
import { Price, ProductDimensions, ProductSpecs, SectionTitle } from '../../ProductList.styles.ts';
import { Button, Card, CardActions, CardContent, CardHeader } from '@mui/material';
import styled from 'styled-components';

interface GroomingCardProps {
  product: components['schemas']['GetGroomingAndHygieneResponse'];
}

export const GroomingAndHygieneCard = ({ product }: GroomingCardProps) => {
  return (
    <Card>
      <CardHeader title={<h3>{product.name}</h3>} subheader={<Price>${product.price}</Price>} />

      <CardContent>
        <ProductSpecs>
          <p>{product.isCrueltyFree && <strong>Is Cruelty Free</strong>}</p>
          <p>{product.isHypoAllergenic && <strong>Is Hypoallergenic</strong>}</p>
          <p>{product.isNatural && <strong>Made from natural ingredients</strong>}</p>
        </ProductSpecs>

        <ProductDimensions>
          <p>
            <strong>Dimensions:</strong> {product.dimensions.length} x {product.dimensions.width} x{' '}
            {product.dimensions.height}
          </p>
        </ProductDimensions>

        <WarningSection>
          <SectionTitle>Safety Warnings:</SectionTitle>
          <p>{product.safetyWarnings}</p>
        </WarningSection>

        <UsageSection>
          <SectionTitle>Usage Instructions:</SectionTitle>
          <p>{product.usageInstructions}</p>
        </UsageSection>
      </CardContent>

      <CardActions sx={{ justifyContent: 'space-evenly' }}>
        <Button variant="contained">Add to cart</Button>
        <Button variant="outlined">View Details</Button>
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
