import {components} from "./shared/types";
import {$api} from "./shared/fetch-client.ts";

export interface PetProductItemProps {
    product: components["schemas"]["GetPetFoodResponse"]
}

export const ProductList = () => {
    const {data, isLoading} = $api.useQuery("get", "/products");

    if (isLoading) {
        return <div>Loading...</div>;
    }

    function isPetFoodResponse(
        product: components["schemas"]["GetProductResponse"]
    ): product is components["schemas"]["GetPetFoodResponse"] {
        return product.category === "PetFood" && "ageGroup" in product && "ingredients" in product;
    }

    function isGroomingAndHygieneResponse(
        product: components["schemas"]["GetProductResponse"]
    ): product is components["schemas"]["GetGroomingAndHygieneResponse"] {
        return product.category === "GroomingAndHygiene" && "isNatural" in product && "usageInstructions" in product;
    }

    const renderProduct = (product:
                           (components["schemas"]["GetProductResponse"]
                               | components["schemas"]["GetPetFoodResponse"]
                               | components["schemas"]["GetGroomingAndHygieneResponse"])) => {
        switch (product.category) {
            case "PetFood":
                if (isPetFoodResponse(product)) {
                    return (<div>{product.name}</div>)
                }
                break;
            case "GroomingAndHygiene":
                if (isGroomingAndHygieneResponse(product)) {
                    return (<div>Grooming and Hygiene!</div>)
                }
                break;
            default:
                return (<div>Base product</div>)
        }
        return <div>Unknown product category</div>;
    }

    return (
        <div>
            {data?.results?.map((product) => renderProduct(product))}
        </div>
    );
};
