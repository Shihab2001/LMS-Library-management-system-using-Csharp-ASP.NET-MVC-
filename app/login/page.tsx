"use client"

import type React from "react"

import { useState } from "react"
import { useRouter } from "next/navigation"
import Link from "next/link"
import { BookOpen } from "lucide-react"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { toast } from "@/components/ui/use-toast"
import { Card, CardContent, CardFooter } from "@/components/ui/card"

export default function LoginPage() {
  const router = useRouter()
  const [isLoading, setIsLoading] = useState(false)
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  })

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target
    setFormData((prev) => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsLoading(true)

    // Simulate authentication
    setTimeout(() => {
      if (formData.email === "admin@library.com" && formData.password === "password") {
        toast({
          title: "Login Successful",
          description: "Welcome to the Library Management System",
        })
        router.push("/")
      } else {
        toast({
          title: "Login Failed",
          description: "Invalid email or password",
          variant: "destructive",
        })
      }
      setIsLoading(false)
    }, 1000)
  }

  return (
    <div className="container flex h-screen w-screen flex-col items-center justify-center">
      <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
        <div className="flex flex-col space-y-2 text-center">
          <div className="flex justify-center">
            <BookOpen className="h-10 w-10 text-primary" />
          </div>
          <h1 className="text-2xl font-semibold tracking-tight">Library Management System</h1>
          <p className="text-sm text-muted-foreground">Enter your credentials to access the system</p>
        </div>
        <Card>
          <form onSubmit={handleSubmit}>
            <CardContent className="pt-6 pb-4">
              <div className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="email">Email</Label>
                  <Input
                    id="email"
                    name="email"
                    placeholder="admin@library.com"
                    type="email"
                    value={formData.email}
                    onChange={handleChange}
                    required
                  />
                </div>
                <div className="space-y-2">
                  <div className="flex items-center justify-between">
                    <Label htmlFor="password">Password</Label>
                    <Link href="/forgot-password" className="text-xs text-muted-foreground hover:text-primary">
                      Forgot password?
                    </Link>
                  </div>
                  <Input
                    id="password"
                    name="password"
                    type="password"
                    value={formData.password}
                    onChange={handleChange}
                    required
                  />
                </div>
                <Button className="w-full" type="submit" disabled={isLoading}>
                  {isLoading ? "Signing in..." : "Sign In"}
                </Button>
              </div>
            </CardContent>
          </form>
          <CardFooter className="border-t px-6 py-4">
            <div className="text-xs text-muted-foreground text-center w-full">
              Demo credentials: admin@library.com / password
            </div>
          </CardFooter>
        </Card>
      </div>
    </div>
  )
}
