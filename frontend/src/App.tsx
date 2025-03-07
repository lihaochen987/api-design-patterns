import { ProductList } from './Pages/Home/ProductList/ProductList.tsx';
import Navbar from './Shared/Navbar/Navbar.tsx';
import { createGlobalStyle } from 'styled-components';

const GlobalStyles = createGlobalStyle`
  html, body {
    margin: 0;
    padding: 0;
    border: 0;
    vertical-align: baseline;
    box-sizing: border-box;
  }
  
  body {
    line-height: normal;
  }
`;

function App() {
  return (
    <>
      <GlobalStyles />
      <Navbar />
      <ProductList />
    </>
  );
}

export default App;
