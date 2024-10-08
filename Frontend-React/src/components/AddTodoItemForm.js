import { Button, Container, Row, Col, Form, Stack } from 'react-bootstrap';
import React, { useState } from 'react';

const AddTodoItemForm = ({onSave, onError}) => {
  const [description, setDescription] = useState('');

  async function handleAdd() {
    try {
      const response = await fetch('https://localhost:44397/api/todoitems', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({          
          description: description
        }),
      });
     
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }      
      
      // Clear form after successful submission      
      setDescription('');
      
      onSave();
    } catch (error) {
      console.error(error);
      onError(error.message);
    }
  }

  const handleDescriptionChange = (event) => {
    event.preventDefault();
    setDescription(event.target.value);
  }

  function handleClear() {
    setDescription('')
  }

  return (
    <Container>
      <h1>Add Item</h1>
      <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
        <Form.Label column sm="2">
          Description
        </Form.Label>
        <Col md="6">
          <Form.Control
            type="text"
            placeholder="Enter description..."
            value={description}
            onChange={handleDescriptionChange}
          />
        </Col>
      </Form.Group>
      <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
        <Stack direction="horizontal" gap={2}>
          <Button variant="primary" onClick={() => handleAdd()}>
            Add Item
          </Button>
          <Button variant="secondary" onClick={() => handleClear()}>
            Clear
          </Button>
        </Stack>
      </Form.Group>
    </Container>
  );
};

export default AddTodoItemForm;