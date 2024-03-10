'use client';

import { loginUser } from '@/services/actions/loginUser';
import React from 'react';
import { useRef } from 'react';

const Login = () => {
  const ref = useRef<HTMLFormElement>(null);

  return (
    <form
      ref={ref}
      action={async (formData) => {
        await loginUser(formData);
        ref.current?.reset();
      }}
    >
      <input type='email' name='email' placeholder='Email' required />
      <input type='password' name='password' placeholder='Password' required />
      <button type='submit'>Login</button>
    </form>
  );
};

export default Login;
