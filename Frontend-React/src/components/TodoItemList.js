import { Button, Table } from 'react-bootstrap';
import React from 'react';

const TodoItemList = ({todoItems, onRefresh, onSave, onError}) => {
    async function handleMarkAsComplete(item) {        
        try {
            const response = await fetch(`https://localhost:44397/api/todoitems/${item.id}`, {
                method: 'PUT',
                headers: {
                  'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                  id: item.id,          
                  description: item.description,
                  isCompleted: true
                }),
              });
        
              alert(JSON.stringify(response.status));
              if (!response.ok) {
                throw new Error('Network response was not ok');
              };                             
              
              onSave();
        } catch (error) {
          console.error(error);
          onError();
        }
      };

    return (
        <>
          <h1>
            Showing {todoItems.length} Item(s){' '}
            <Button variant="primary" className="pull-right" onClick={() => onRefresh()}>
              Refresh
            </Button>
          </h1>        
          <Table striped bordered hover>
            <thead>
              <tr>
                <th>Id</th>
                <th>Description</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {todoItems.map((item) => (
                <tr key={item.id}>
                  <td>{item.id}</td>
                  <td>{item.description}</td>
                  <td>
                    <Button variant="warning" size="sm" onClick={() => handleMarkAsComplete(item)}>
                      Mark as completed
                    </Button>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>
        </>
      )
};

export default TodoItemList;