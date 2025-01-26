import {$api} from "@repo/product-data-access/api";
import React from 'react';
import {ShoppingList} from "./ShoppingList/shopping-list.tsx";

function App() {
    const {data, isLoading} = $api.useQuery(
        "get",
        "/products"
    );

    console.log(data);

    if (isLoading || !data) return "Loading...";

    return (
        <>
            <h1>Welcome to the Pet store</h1>
            <ShoppingList/>
        </>
    )
}

export default App
