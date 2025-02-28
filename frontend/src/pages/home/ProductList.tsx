import { components } from "../../shared/types";
import { $api } from "../../shared/fetch-client.ts";
import {PetFoodCard} from "./cards/PetFoodCard.tsx";
import {GroomingAndHygieneCard} from "./cards/GroomingAndHygieneCard.tsx";
import {DefaultProductCard} from "./cards/DefaultProductCard.tsx";
import {ProductListContainer} from "./ProductList.styles.ts";

export const ProductList = () => {
    const { data, isLoading } = $api.useQuery("get", "/products");

    if (isLoading) {
        return <div>Loading...</div>;
    }

    const renderProduct = (product:
                               components["schemas"]["GetProductResponse"] |
                               components["schemas"]["GetPetFoodResponse"] |
                               components["schemas"]["GetGroomingAndHygieneResponse"]) => {
        switch (product.category) {
            case "PetFood":
                return <PetFoodCard product={product as components["schemas"]["GetPetFoodResponse"]} />;
            case "GroomingAndHygiene":
                return <GroomingAndHygieneCard product={product as components["schemas"]["GetGroomingAndHygieneResponse"]} />;
            default:
                return <DefaultProductCard product={product} />;
        }
    };

    return (
        <ProductListContainer>
            {data?.results?.map((product) => (
                <div key={product.id} className="product-item">
                    {renderProduct(product)}
                </div>
            ))}
        </ProductListContainer>
    );
};
