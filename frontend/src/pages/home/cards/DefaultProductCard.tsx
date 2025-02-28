import { components } from "../../../shared/types";
import {
    ProductCard,
    ProductHeader,
    Price,
    ProductDetails,
    ProductDimensions,
    ProductActions,
    AddToCartButton,
    ViewDetailsButton
} from "./ProductCard.styles.ts";

interface DefaultProductCardProps {
    product: components["schemas"]["GetProductResponse"];
}

export const DefaultProductCard: React.FC<DefaultProductCardProps> = ({ product }) => {
    return (
        <ProductCard variant="default">
            <ProductHeader>
                <h3>{product.name}</h3>
                <Price>${product.price}</Price>
            </ProductHeader>

            <ProductDetails>
                <p><strong>Category:</strong> {product.category}</p>

                <ProductDimensions>
                    <p><strong>Dimensions:</strong> {product.dimensions.length} x {product.dimensions.width} x {product.dimensions.height}</p>
                </ProductDimensions>
            </ProductDetails>

            <ProductActions>
                <AddToCartButton>Add to Cart</AddToCartButton>
                <ViewDetailsButton>View Details</ViewDetailsButton>
            </ProductActions>
        </ProductCard>
    );
};
