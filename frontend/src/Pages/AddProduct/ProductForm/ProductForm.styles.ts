import styled from 'styled-components';

export const PageContainer = styled.div`
  margin: 100px auto 50px;
  padding: 0 25rem;
`;

export const PageTitle = styled.h1`
  font-size: 2rem;
  color: #333;
  margin-bottom: 1.75rem;
`;

export const Form = styled.form`
  background-color: #fff;
  padding: 1.75rem;
  border-radius: 8px;
  border: 1px solid red;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
`;

export const FormGroup = styled.div`
  margin-bottom: 1.25rem;
`;

export const Label = styled.label`
  display: block;
  font-weight: 500;
  margin-bottom: 0.5rem;
  color: #333;
`;

export const Input = styled.input`
  width: 100%;
  padding: 0.5rem 1rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.3s;

  &:focus {
    border-color: #4a90e2;
    outline: none;
  }
`;

export const Select = styled.select`
  width: 100%;
  padding: 0.5rem 1rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.3s;

  &:focus {
    border-color: #4a90e2;
    outline: none;
  }
`;

export const ButtonGroup = styled.div`
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 1.75rem;
`;

export const Button = styled.button`
  padding: 0.5rem 1.25rem;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.3s;
`;

export const PrimaryButton = styled(Button)`
  background-color: #4a90e2;
  color: white;

  &:hover {
    background-color: #3a80d2;
  }
`;

export const SecondaryButton = styled(Button)`
  background-color: #f5f5f5;
  color: #333;

  &:hover {
    background-color: #e5e5e5;
  }
`;

export const ErrorMessage = styled.p`
  color: #e53e3e;
  font-size: 1rem;
  margin-top: 0.5rem;
`;

export const LoadingMessage = styled.div`
  background-color: #e8f4fd;
  padding: 0.75rem 1rem;
  border-radius: 4px;
  margin-bottom: 1.25rem;
  color: #2c7cb0;
`;

export const ErrorBanner = styled.div`
  background-color: #fff5f5;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  margin-bottom: 1.25rem;
  color: #e53e3e;
  border-left: 3px solid #e53e3e;
`;
