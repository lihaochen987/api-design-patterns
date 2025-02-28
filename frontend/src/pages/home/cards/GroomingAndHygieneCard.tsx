import { components } from '../../../shared/types';
import { Price } from '../ProductList.styles.ts';
import { Button, Card, CardActions, CardHeader } from '@mui/material';

interface GroomingCardProps {
  product: components['schemas']['GetGroomingAndHygieneResponse'];
}

export const GroomingAndHygieneCard = ({ product }: GroomingCardProps) => {
  return (
    <Card>
      <CardHeader title={<h3>{product.name}</h3>} subheader={<Price>${product.price}</Price>} />

      <div>
        <p>Grooming and Hygiene Product</p>
        {/* Additional GroomingAndHygiene specific fields can be added here */}
      </div>

      <CardActions sx={{ justifyContent: 'space-evenly' }}>
        <Button variant="contained">Add to cart</Button>
        <Button variant="outlined">View Details</Button>
      </CardActions>
    </Card>
  );
};
