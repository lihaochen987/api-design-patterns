import PetsIcon from '@mui/icons-material/Pets';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import {
  NavbarContainer,
  LogoContainer,
  Title,
  NavItems,
  NavItem,
  CartIconWrapper,
  CartBadge,
  NavbarContent,
  NavigationControls,
} from './Navbar.styles';

interface NavbarProps {
  cartItemsCount?: number;
}

const Navbar = ({ cartItemsCount = 0 }: NavbarProps) => {
  return (
    <NavbarContainer>
      <NavbarContent>
        <LogoContainer>
          <PetsIcon />
          <Title>The Petstore</Title>
        </LogoContainer>

        <NavigationControls>
          <NavItems>
            <NavItem>Products</NavItem>
            <NavItem>Services</NavItem>
            <NavItem>About Us</NavItem>
            <NavItem>Contact</NavItem>
          </NavItems>

          <CartIconWrapper>
            <ShoppingCartIcon />
            {cartItemsCount > 0 && <CartBadge>{cartItemsCount}</CartBadge>}
          </CartIconWrapper>
        </NavigationControls>
      </NavbarContent>
    </NavbarContainer>
  );
};

export default Navbar;
