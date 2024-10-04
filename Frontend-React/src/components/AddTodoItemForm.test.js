import React from 'react';
import { render, screen, fireEvent, act, waitFor } from '@testing-library/react';
import AddTodoItemForm from './AddTodoItemForm';

global.fetch = jest.fn();

beforeEach(() => {
    global.fetch.mockClear();
});

afterEach(() => {
    jest.restoreAllMocks();
});

describe('AddTodoItemForm component', () => {
    it('Adding a new item succeeds', async () => {
        const mockOnSave = jest.fn();
        const mockOnError = jest.fn();
        const testDescription = 'Test adding a new todo item';
        const mockFetchResponse = {
            ok: true,
            json: jest.fn().mockResolvedValue({ id: '1', description: testDescription })
          };
          
        fetch.mockResolvedValue(mockFetchResponse);
    
        render(<AddTodoItemForm onSave={mockOnSave} onError={mockOnError} />);
    
        // Fill in the description
        const input = screen.getByPlaceholderText('Enter description...');
        fireEvent.change(input, { target: { value: testDescription} });
    
        // Click the save button        
        const saveButton = screen.getByRole('button', { name: /add item/i });
        fireEvent.click(saveButton);
    
        // Wait for the async operation to complete
        await waitFor(() => {
          expect(fetch).toHaveBeenCalledWith('https://localhost:44397/api/todoitems', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify({ description: testDescription }),
          });
          expect(mockOnSave).toHaveBeenCalledWith();
          expect(mockOnError).not.toHaveBeenCalled();
        });
      });

    it('Adding a new item fails with error', async () => {
        const mockOnSave = jest.fn();
        const mockOnError = jest.fn();
        const testDescription = 'Test adding a new todo item';
        const mockFetchResponse = {
            ok: false            
          };
          
        fetch.mockResolvedValue(mockFetchResponse);
    
        render(<AddTodoItemForm onSave={mockOnSave} onError={mockOnError} />);
    
        // Fill in the description
        const input = screen.getByPlaceholderText('Enter description...');
        fireEvent.change(input, { target: { value: testDescription} });
    
        // Click the save button        
        const saveButton = screen.getByRole('button', { name: /add item/i });
        fireEvent.click(saveButton);
    
        // Wait for the async operation to complete
        await waitFor(() => {
          expect(fetch).toHaveBeenCalledWith('https://localhost:44397/api/todoitems', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify({ description: testDescription }),
          });
          expect(mockOnSave).not.toHaveBeenCalledWith();
          expect(mockOnError).toHaveBeenCalled();
        });
      });
});