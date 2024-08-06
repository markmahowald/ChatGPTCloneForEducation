

async function callApi() {
  const response = await openai.chat.completions.createCompletion({
    messages: [{ role: "system", content: "You are a helpful assistant." }],
    model: "gpt-4o",
  });

  console.log(response.choices[0]);
}

main();



//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