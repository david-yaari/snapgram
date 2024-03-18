'use client';

import { loginUser } from '@/services/actions/loginUser';
import { useRouter } from 'next/navigation';
import { FormEvent, useEffect, useState } from 'react';
import React from 'react';
import { useRef } from 'react';

const Login = () => {
  const router = useRouter();
  const [error, setError] = useState<string | null>(null);

  // Clear the error message whenever the component is re-rendered
  useEffect(() => {
    setError(null);
  }, []);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
    const formData = new FormData(event.currentTarget);

    try {
      await loginUser(formData);
      router.push('/'); // Navigate to the root
    } catch (error: unknown) {
      if (error instanceof Error) {
        if (error.message === 'Unauthorized!') {
          // Handle 401 Unauthorized error
          setError(
            'You are not authorized. Please check your username and password.'
          );
        } else {
          setError(error.message);
        }
      } else {
        setError('An unknown error occurred');
      }
    }
  }

  return (
    <div>
      <form className='text-black' onSubmit={handleSubmit}>
        <input
          // type='email'

          name='email'
          placeholder='Email'
          required
        />
        <input
          type='password'
          name='password'
          placeholder='Password'
          required
        />
        <button className='text-white' type='submit'>
          Login
        </button>
      </form>
      {error && <p>Error: {error}</p>}
    </div>
  );
};

export default Login;
