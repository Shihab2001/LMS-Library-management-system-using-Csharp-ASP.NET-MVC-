import Link from "next/link"
import { BookOpen, Users, BarChart, BookCopy, UserPlus, BookPlus } from "lucide-react"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"

export default function Dashboard() {
  return (
    <div className="flex min-h-screen w-full flex-col">
      <main className="flex-1 p-6 md:p-10">
        <div className="flex flex-col space-y-6">
          <div className="space-y-2">
            <h1 className="text-3xl font-bold tracking-tight">Library Management System</h1>
            <p className="text-muted-foreground">Manage your library's books, members, and transactions efficiently.</p>
          </div>

          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            <Card>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium">Total Books</CardTitle>
                <BookOpen className="h-4 w-4 text-muted-foreground" />
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">1,248</div>
                <p className="text-xs text-muted-foreground">+12 added this month</p>
              </CardContent>
            </Card>
            <Card>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium">Registered Members</CardTitle>
                <Users className="h-4 w-4 text-muted-foreground" />
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">342</div>
                <p className="text-xs text-muted-foreground">+8 new members this month</p>
              </CardContent>
            </Card>
            <Card>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium">Books Issued</CardTitle>
                <BookCopy className="h-4 w-4 text-muted-foreground" />
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">89</div>
                <p className="text-xs text-muted-foreground">24 due this week</p>
              </CardContent>
            </Card>
          </div>

          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            <Link href="/books">
              <Card className="h-full cursor-pointer transition-colors hover:bg-muted/50">
                <CardHeader>
                  <BookOpen className="h-5 w-5 text-primary" />
                  <CardTitle className="mt-2">Books Management</CardTitle>
                  <CardDescription>Add, edit, and manage your book inventory</CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="flex space-x-2">
                    <Button size="sm" variant="outline">
                      <BookPlus className="mr-2 h-4 w-4" />
                      Add New Book
                    </Button>
                    <Button size="sm" variant="outline">
                      View All
                    </Button>
                  </div>
                </CardContent>
              </Card>
            </Link>
            <Link href="/members">
              <Card className="h-full cursor-pointer transition-colors hover:bg-muted/50">
                <CardHeader>
                  <Users className="h-5 w-5 text-primary" />
                  <CardTitle className="mt-2">Members Management</CardTitle>
                  <CardDescription>Register and manage library members</CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="flex space-x-2">
                    <Button size="sm" variant="outline">
                      <UserPlus className="mr-2 h-4 w-4" />
                      Register Member
                    </Button>
                    <Button size="sm" variant="outline">
                      View All
                    </Button>
                  </div>
                </CardContent>
              </Card>
            </Link>
            <Link href="/transactions">
              <Card className="h-full cursor-pointer transition-colors hover:bg-muted/50">
                <CardHeader>
                  <BarChart className="h-5 w-5 text-primary" />
                  <CardTitle className="mt-2">Transactions</CardTitle>
                  <CardDescription>Issue and return books, track due dates</CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="flex space-x-2">
                    <Button size="sm" variant="outline">
                      Issue Book
                    </Button>
                    <Button size="sm" variant="outline">
                      Return Book
                    </Button>
                  </div>
                </CardContent>
              </Card>
            </Link>
          </div>
        </div>
      </main>
    </div>
  )
}
