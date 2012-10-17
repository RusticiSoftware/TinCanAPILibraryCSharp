NOTE: This library is made for TCAPI v0.90, which is not the latest version.  The master branch will contain the latest (currently 0.95) and should be used.  Even if the LRS you're hitting is v0.90, the v0.95 can be used and statements will automatically be converted (although sometimes with data loss) to and from 0.95 to 0.90, meaning when the LRS is updated to 0.95, the library doesn't have to change at all.

This library provides the TCAPI class which allows you to communicate with an LRS (Specified as an endpoint).  If you are looking to simply use TinCan, using the TCAPI Class, making an instance of it and using the provided methods will likely cover all your needs - it provides some simple Async support along with many helper methods.  If you need something more focused or specific to the implementation, implement the ITCAPI interface and build the TCAPI class.

Copyright 2012 Rustici Software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.