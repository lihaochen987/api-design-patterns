import {$api} from "@repo/product-data-access/api";
import React from 'react';

function App() {
    const {data, error, isLoading} = $api.useQuery(
        "get",
        "/product/{id}",
        {
            params: {
                path: {id: 1},
            },
        },
    );

    console.log(data);

    if (isLoading || !data) return "Loading...";

    // eslint-disable-next-line @typescript-eslint/restrict-template-expressions
    if (error) return `An error occurred: ${error.message}`;

    return (
        <>
            <h1>Welcome to the Pet store</h1>
        </>
    )
}

export default App
