import createFetchClient from 'openapi-fetch';
import { paths } from './types';
import { QueryClient } from '@tanstack/react-query';

export const fetchClient = createFetchClient<paths>({
  baseUrl: 'http://localhost:8080/',
});

export const queryClient = new QueryClient();
