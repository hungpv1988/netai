using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using SampleMCPServer.Model;

namespace SampleMCPServer.Tools
{
    internal class CreateFormTool
    {
        [McpServerTool(Name = "create_form")]
        [Description("""
        This tool creates a new form in the system based on extracted form data.

        ### Expected Input
        The parameter must match the `FormModel` schema:
        {
          "context": "string that describes what the form is for",
          "numberOfRows": int,
          "formStructure": [
            {
              "numberOfFields": int,
              "fields": [
                { "label": "string", "inputType": "string", "options": ["..."] }
              ]
            }
          ]
        }

        Example:  {
          "context": "This is a form used to register a new user for a campaign.",
          "numberOfRows": "3",
          "formStructure": [
            {
              "numberOfFields": 2,
              "fields": [
                {
                  "label": "Name",
                  "inputType": "text box",
                  "options": []
                },
                {
                  "label": "Email",
                  "inputType": "text box",
                  "options": []
                }
              ]
            },
            {
              "numberOfFields": 1,
              "fields": [
                {
                  "label": "Age Group",
                  "inputType": "dropdown",
                  "options": ["18-24", "25-34", "35-44", "45+"]
                }
              ]
            },
            {
              "numberOfFields": 1,
              "fields": [
                {
                  "label": "Subscribe to newsletter",
                  "inputType": "checkbox",
                  "options": []
                }
              ]
            }
          ]
        }
        ### Usage Instruction for the LLM
        - If the user uploads an image of a form, analyze the image to extract all fields and metadata.
        - Convert that extracted information into a valid `FormModel` object.
        - Then call this tool (`create_form`) with the generated data.
        - Do not ask the user for clarification if the form fields can be inferred from the image.
        """)]
        public Form CreateForm([Description("Form model extracted from the image that user upload")] Form form)
        {
            // Simulate form creation logic
            form.Id = Guid.NewGuid(); // Assign a new ID to the form
            return form;
        }
    }
}
