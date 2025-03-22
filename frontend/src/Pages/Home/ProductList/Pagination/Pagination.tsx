import styled from 'styled-components';

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

export const Pagination = ({ currentPage, totalPages, onPageChange }: PaginationProps) => {
  const getPageNumbers = () => {
    const pageNumbers = [];
    const maxPagesToShow = 5;

    if (totalPages <= maxPagesToShow) {
      for (let i = 1; i <= totalPages; i++) {
        pageNumbers.push(i);
      }
    } else {
      pageNumbers.push(1);

      let startPage = Math.max(2, currentPage - 1);
      let endPage = Math.min(totalPages - 1, currentPage + 1);

      if (currentPage <= 2) {
        endPage = 4;
      } else if (currentPage >= totalPages - 1) {
        startPage = totalPages - 3;
      }

      if (startPage > 2) {
        pageNumbers.push('ellipsis-start');
      }

      for (let i = startPage; i <= endPage; i++) {
        pageNumbers.push(i);
      }

      if (endPage < totalPages - 1) {
        pageNumbers.push('ellipsis-end');
      }

      pageNumbers.push(totalPages);
    }

    return pageNumbers;
  };

  return (
    <PaginationWrapper>
      <PaginationButton
        onClick={() => onPageChange(1)}
        $isDisabled={currentPage === 1}
        disabled={currentPage === 1}
        aria-label="Go to first page"
      >
        &laquo;
      </PaginationButton>

      <PaginationButton
        onClick={() => onPageChange(currentPage - 1)}
        $isDisabled={currentPage === 1}
        disabled={currentPage === 1}
        aria-label="Go to previous page"
      >
        &lsaquo;
      </PaginationButton>

      {getPageNumbers().map((page, index) => {
        if (page === 'ellipsis-start' || page === 'ellipsis-end') {
          return <PaginationEllipsis key={`ellipsis-${index}`}>...</PaginationEllipsis>;
        }

        return (
          <PaginationButton
            key={`page-${page}`}
            $isActive={currentPage === page}
            onClick={() => onPageChange(Number(page))}
            aria-label={`Go to page ${page}`}
            aria-current={currentPage === page ? 'page' : undefined}
          >
            {page}
          </PaginationButton>
        );
      })}

      <PaginationButton
        onClick={() => onPageChange(currentPage + 1)}
        $isDisabled={currentPage === totalPages}
        disabled={currentPage === totalPages}
        aria-label="Go to next page"
      >
        &rsaquo;
      </PaginationButton>

      <PaginationButton
        onClick={() => onPageChange(totalPages)}
        $isDisabled={currentPage === totalPages}
        disabled={currentPage === totalPages}
        aria-label="Go to last page"
      >
        &raquo;
      </PaginationButton>
    </PaginationWrapper>
  );
};

const PaginationWrapper = styled.div`
  display: flex;
  align-items: center;
  gap: 0.5rem;
`;

const PaginationButton = styled.button<{ $isActive?: boolean; $isDisabled?: boolean }>`
  min-width: 2.5rem;
  padding: 0.5rem 0 0.5rem 0;
  border-radius: 4px;
  border: 1px solid ${props => (props.$isActive ? '#1976d2' : '#ddd')};
  background-color: ${props => (props.$isActive ? '#1976d2' : 'white')};
  color: ${props => (props.$isActive ? 'white' : props.$isDisabled ? '#ccc' : '#333')};
  font-size: 1rem;
  cursor: ${props => (props.$isDisabled ? 'not-allowed' : 'pointer')};
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;

  &:hover {
    background-color: ${props =>
      props.$isDisabled ? 'white' : props.$isActive ? '#1565c0' : '#f5f5f5'};
  }

  &:focus {
    outline: none;
    box-shadow: 0 0 0 2px rgba(25, 118, 210, 0.3);
  }
`;

const PaginationEllipsis = styled.span`
  display: flex;
  align-items: center;
  justify-content: center;
  min-width: 2.5rem;
  padding: 0.5rem 0 0.5rem 0;
  color: #666;
`;
