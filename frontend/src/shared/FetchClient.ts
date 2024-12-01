import createFetchClient from "openapi-fetch";
import createClient from "openapi-react-query";
import {paths} from "../../types";

const fetchClient = createFetchClient<paths>({
    baseUrl: "http://localhost:8080/",
});

export const $api = createClient(fetchClient);