import './App.css';
import UserRegistration from './Pages/UserRegistration';

const App = () => {

  const router = createBrowserRouter([
    {
      path: "/",
      element: <Layout/>,
      errorElement: <ErrorPage />,
      children: [
        {
          path: "/",
          element: <div></div>
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

export default App;
