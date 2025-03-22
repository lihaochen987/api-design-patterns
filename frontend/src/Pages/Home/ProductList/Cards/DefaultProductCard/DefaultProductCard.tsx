import { components } from '../../../../../Shared/types';
import {
  Card,
  CardActions,
  CardButton,
  CardContent,
  CardHeader,
  Price,
  ProductDimensions,
} from '../../ProductList.styles.ts';

interface DefaultProductCardProps {
  product: components['schemas']['GetProductResponse'];
}

export const DefaultProductCard = ({ product }: DefaultProductCardProps) => {
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
      </CardContent>

      <CardActions>
        <CardButton variant="contained">Add to cart</CardButton>
      </CardActions>
    </Card>
  );
};
