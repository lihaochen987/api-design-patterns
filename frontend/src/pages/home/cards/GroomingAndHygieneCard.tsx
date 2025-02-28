import { components } from "../../../shared/types";
import {
    ProductCard,
    ProductHeader,
    Price,
    ProductActions,
    AddToCartButton,
    ViewDetailsButton
} from "./ProductCard.styles.ts";

interface GroomingCardProps {
    product: components["schemas"]["GetGroomingAndHygieneResponse"];
}

export const GroomingCard: React.FC<GroomingCardProps> = ({ product }) => {
    return (
        <ProductCard variant="grooming">
            <ProductHeader>
                <h3>{product.name}</h3>
                <Price>${product.price}</Price>
            </ProductHeader>

            <div>
                <p>Grooming and Hygiene Product</p>
                {/* Additional GroomingAndHygiene specific fields can be added here */}
            </div>

            <ProductActions>
                <AddToCartButton>Add to Cart</AddToCartButton>
                <ViewDetailsButton>View Details</ViewDetailsButton>
            </ProductActions>
        </ProductCard>
    );
};