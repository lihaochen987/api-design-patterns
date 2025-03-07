import styled from 'styled-components';

export const NavbarContainer = styled.header`
  display: flex;
  padding: 0.75rem;
  background-color: #1976d2;
  color: white;
  box-shadow:
    0 2px 4px -1px rgba(0, 0, 0, 0.2),
    0 4px 5px 0 rgba(0, 0, 0, 0.14),
    0 1px 10px 0 rgba(0, 0, 0, 0.12);
  position: sticky;
  top: 0;
  z-index: 1100;
`;

export const LogoContainer = styled.div`
  display: flex;
  align-items: center;
  flex-grow: 1;

  svg {
    margin-right: 0.5rem;
    font-size: 1.5rem;
  }
`;

export const Title = styled.h1`
  margin: 0;
  font-size: 1.25rem;
  font-weight: 600;
  letter-spacing: 1px;
  white-space: nowrap;
`;

export const NavItems = styled.nav`
  display: flex;
  gap: 1rem;
  margin-right: 1rem;

  a {
    transform: translateY(3px);
  }
`;

export const NavItem = styled.a`
  color: white;
  cursor: pointer;
  font-weight: 500;
  text-decoration: none;
  padding: 0.5rem 0.75rem;
  border-radius: 0.25rem;
  transition: background-color 0.3s;

  &:hover {
    background-color: rgba(255, 255, 255, 0.1);
  }
`;

export const CartIconWrapper = styled.div`
  position: relative;
  cursor: pointer;
  padding: 0.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  transition: background-color 0.3s;

  &:hover {
    background-color: rgba(255, 255, 255, 0.1);
  }
`;

export const CartBadge = styled.div`
  position: absolute;
  top: 0;
  right: 0;
  background-color: #f50057;
  color: white;
  border-radius: 50%;
  width: 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 0.75rem;
  font-weight: bold;
`;

export const NavbarContent = styled.div`
  display: flex;
  justify-content: space-between;
  width: 100%;
`;

export const NavigationControls = styled.div`
  display: flex;
`;
