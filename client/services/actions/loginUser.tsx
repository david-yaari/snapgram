'use server';

import { cookies } from 'next/headers';

export const loginUser = async (FormData: FormData) => {
  'use server';

  const email = FormData.get('email');
  const password = FormData.get('password');

  const UserName = email;
  const Password = password;
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

  try {
    const response = await fetch('https://localhost:5501/authenticate', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ UserName, Password }),
    });

    if (!response.ok) {
      let errorMessage = `HTTP error! status: ${response.status}`;

      if (response.status === 401) {
        errorMessage = 'Unauthorized!';
      } else if (response.status === 500) {
        errorMessage = 'Internal server error!';
      }

      throw new Error(errorMessage);
    }

    const data = await response.json();
    const jwtToken = data.jwtToken;

    // Create the session
    const expires = new Date(Date.now() + 15 * 60 * 1000);
    // Save the session in a cookie
    cookies().set('session', jwtToken, { expires, httpOnly: true });
  } catch (error) {
    console.error('An error occurred:', error);
    throw error; // Re-throw the error so it can be caught and handled by the calling code
  }
};
