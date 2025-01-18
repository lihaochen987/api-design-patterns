# React + TypeScript + Vite

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react/README.md)
  uses [Babel](https://babeljs.io/) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react-swc) uses [SWC](https://swc.rs/) for Fast
  Refresh

## Generating types

1. Install openapi-typescript
2. Create a `.json` with the OpenApi schema inside of it
3. Run the command `openapi-typescript .\schema.json --output types.d.ts`
4. Run the command `eslint --fix` since the generated file will have some linting errors