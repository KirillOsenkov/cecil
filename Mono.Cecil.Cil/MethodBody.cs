//
// MethodBody.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2010 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

using Mono.Collections.Generic;

namespace Mono.Cecil.Cil {

	public sealed class MethodBody : IVariableDefinitionProvider {

		readonly internal MethodDefinition method;

		internal ParameterDefinition this_parameter;
		internal int max_stack_size;
		internal int code_size;
		internal bool init_locals;
		internal MetadataToken local_var_token;

		internal Collection<Instruction> instructions;
		internal Collection<ExceptionHandler> exceptions;
		internal Collection<VariableDefinition> variables;
		Scope scope;

		public MethodDefinition Method {
			get { return method; }
		}

		public int MaxStackSize {
			get { return max_stack_size; }
			set { max_stack_size = value; }
		}

		public int CodeSize {
			get { return code_size; }
		}

		public bool InitLocals {
			get { return init_locals; }
			set { init_locals = value; }
		}

		public MetadataToken LocalVarToken {
			get { return local_var_token; }
			set { local_var_token = value; }
		}

		public Collection<Instruction> Instructions {
			get {
				if (instructions == null)
					instructions = new Collection<Instruction> ();

				return instructions;
			}
		}

		public bool HasExceptionHandlers {
			get { return !exceptions.IsNullOrEmpty (); }
		}

		public Collection<ExceptionHandler> ExceptionHandlers {
			get {
				if (exceptions == null)
					exceptions = new Collection<ExceptionHandler> ();

				return exceptions;
			}
		}

		public bool HasVariables {
			get { return !variables.IsNullOrEmpty (); }
		}

		public Collection<VariableDefinition> Variables {
			get {
				if (variables == null)
					variables = new VariableDefinitionCollection ();

				return variables;
			}
		}

		public Scope Scope {
			get { return scope; }
			set { scope = value; }
		}

		public ParameterDefinition ThisParameter {
			get {
				if (method == null || method.DeclaringType == null)
					throw new NotSupportedException ();

				if (this_parameter == null)
					this_parameter = new ParameterDefinition ("0", ParameterAttributes.None, method.DeclaringType);

				return this_parameter;
			}
		}

		public MethodBody (MethodDefinition method)
		{
			this.method = method;
		}

		public ILProcessor GetILProcessor ()
		{
			return new ILProcessor (this);
		}
	}

	public interface IVariableDefinitionProvider {
		bool HasVariables { get; }
		Collection<VariableDefinition> Variables { get; }
	}

	class VariableDefinitionCollection : Collection<VariableDefinition> {

		internal VariableDefinitionCollection ()
		{
		}

		internal VariableDefinitionCollection (int capacity)
			: base (capacity)
		{
		}

		protected override void OnAdd (VariableDefinition item, int index)
		{
			item.index = index;
		}

		protected override void OnInsert (VariableDefinition item, int index)
		{
			item.index = index;

			for (int i = index; i < this.Count; i++)
				this [i].index = i + 1;
		}

		protected override void OnSet (VariableDefinition item, int index)
		{
			item.index = index;
		}

		protected override void OnRemove (VariableDefinition item, int index)
		{
			item.index = -1;

			for (int i = index + 1; i < this.Count; i++)
				this [i].index = i - 1;
		}
	}
}
