# Hello, SNS!

This is the code project for the [Hello, SNS!](https://davidpallmann.hashnode.dev/hello-sns) blog post. 

This episode: Amazon SNS. In this [Hello, Cloud](https://davidpallmann.hashnode.dev/series/hello-cloud) blog series, we're covering the basics of AWS cloud services for newcomers who are .NET developers. If you love C# but are new to AWS, or to this particular service, this should give you a jumpstart.

In this post we'll introduce Amazon SNS and use it in a "Hello, Cloud" .NET program. We'll do this step-by-step, making no assumptions other than familiarity with C# and Visual Studio. We're using Visual Studio 2022 and .NET 6.

## Our Hello, SNS Project

We’re going to create a topic in the AWS console and configure a subscription for an SQS queue. Then, we'll create a .NET 6 console program that can 1) send notifications (a publisher), and 2) receive notifications via SQS (a subscriber). We'll also add a second subscription for human notification by email. Each notification we publish will result in two subscriber notifications.

See the blog post for the tutorial to create this project and run it on AWS.

