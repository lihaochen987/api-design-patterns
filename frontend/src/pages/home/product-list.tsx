import {components} from "../../shared/types";
import {$api} from "../../shared/fetch-client.ts";

export const ProductList = () => {
    const {data, isLoading} = $api.useQuery("get", "/products");

    if (isLoading) {
        return <div>Loading...</div>;
    }

    const renderProduct = (product:
                           (components["schemas"]["GetProductResponse"]
                               | components["schemas"]["GetPetFoodResponse"]
                               | components["schemas"]["GetGroomingAndHygieneResponse"])) => {
        switch (product.category) {
            case "PetFood":
                return (<div>{product.name}</div>)
            case "GroomingAndHygiene":
                return (<div>Grooming and Hygiene!</div>)
            default:
                return (<div>Base product</div>)
        }
    }

    return (
        <div>
            {data?.results?.map((product) => renderProduct(product))}
        </div>
    );
};
