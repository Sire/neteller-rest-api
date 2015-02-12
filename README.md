Neteller REST API .NET Wrapper
==============================

Supported API version: v1 (2014-11-04, patched 2014-12-09)


Get Started
-----------
1. Copy Neteller.config.sample to Neteller.config and add your Neteller App Client ID and Secret.
2. Properties on Neteller.config, "Copy to output directory" should be set to "Copy if newer".
   This ensures it's copied with the Test project as well.
3. Log in to Neteller and verify API is enabled, an application is created correctly and that your IP is allowed.
4. Do the same for Neteller Sandbox (testing environment).
5. Build and make sure all unit tests succeed.


Todo
----

Create more unit tests.
Create deserialization tests for all model entities.
Change all model class properties into correct case, and add [DeserializeAs(Name = "jsonPropertyName")] to all.
Add XML comments where needed.


Changelog
---------

2015-02-12 First beta release. Subscriptions works.


Simon