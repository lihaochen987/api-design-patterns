import React, { useState } from 'react';
import PetsIcon from '@mui/icons-material/Pets';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import MenuIcon from '@mui/icons-material/Menu';
import {
  NavbarContainer,
  LogoContainer,
  Title,
  NavItems,
  NavItem,
  CartIconWrapper,
  CartBadge,
  MobileMenuButton,
  NavbarToolbar,
} from './Navbar.styles';

interface NavbarProps {
  onMenuClick?: () => void;
  cartItemsCount?: number;
}

const Navbar: React.FC<NavbarProps> = ({ onMenuClick, cartItemsCount = 0 }) => {
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);

  React.useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth < 768);
    };

    window.addEventListener('resize', handleResize);
    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  return (
    <NavbarContainer>
      <NavbarToolbar>
        {isMobile && (
          <MobileMenuButton onClick={onMenuClick}>
            <MenuIcon />
          </MobileMenuButton>
        )}

        <LogoContainer>
          <PetsIcon />
          <Title>The Pet store</Title>
        </LogoContainer>

        {!isMobile && (
          <NavItems>
            <NavItem>Products</NavItem>
            <NavItem>Services</NavItem>
            <NavItem>About Us</NavItem>
            <NavItem>Contact</NavItem>
          </NavItems>
        )}

        <CartIconWrapper>
          <ShoppingCartIcon />
          {cartItemsCount > 0 && <CartBadge>{cartItemsCount}</CartBadge>}
        </CartIconWrapper>
      </NavbarToolbar>
    </NavbarContainer>
  );
};

export default Navbar;
