'use client';

import { FormEvent } from 'react';
import { useRouter } from 'next/navigation';
import Login from '@/components/Login';

export default function Page() {
  const router = useRouter();

  return <Login />;
}
