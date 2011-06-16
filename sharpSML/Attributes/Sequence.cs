/*
    This file is part of sharpSML, Copyright 2011 Stefan Schake.

    sharpSML is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    sharpSML is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with sharpSML.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

namespace sharpSML.Attributes
{

    [AttributeUsage(AttributeTargets.Struct)]
    public class Sequence : Attribute
    {
        // instructs marshal to read all fields sequentially
    }

}