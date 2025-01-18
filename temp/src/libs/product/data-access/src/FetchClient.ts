import createFetchClient from "openapi-fetch";
import createClient from "openapi-react-query";
import {paths} from "../../../shared/utility/src/types";

const fetchClient = createFetchClient<paths>({
    baseUrl: "http://localhost:8080/",
});

export const $api = createClient(fetchClient);