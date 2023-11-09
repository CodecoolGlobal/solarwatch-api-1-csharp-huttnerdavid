import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';

const App = () => {

  const router = createBrowserRouter([
    {
      path: "/",
      element: <Layout/>,
      errorElement: <ErrorPage />,
      children: [
        {
          path: "/",
          element: <WelcomePage />
        },
        {
          path: "/registration",
          element: <UserRegistration />,
        }
      ],
    },
  ]);

  return (
    <React.StrictMode>
      <RouterProvider router={router}>
        <Layout />
      </RouterProvider>
    </React.StrictMode>
  );
};

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(<App />);
