'use server';

export const loginUser = async (FormData: FormData) => {
  'use server';

  const email = FormData.get('email');
  const password = FormData.get('password');

  const response = await fetch('/api/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email, password }),
  });
};
