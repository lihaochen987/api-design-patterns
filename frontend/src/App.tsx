import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Navbar from './Shared/Navbar/Navbar.tsx';
import { createGlobalStyle } from 'styled-components';
import { HomePage } from './Pages/Home/HomePage.tsx';
import { AddProductPage } from './Pages/AddProduct/AddProductPage.tsx';

const App = () => {
  return (
    <BrowserRouter>
      <GlobalStyles />
      <Navbar cartItemsCount={0} />
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/products/add" element={<AddProductPage />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;

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
