let RemoveBtns = document.querySelectorAll(".rem-btn");
let totalPriceInner = document.querySelector("#totalPrice")
let totalPrice = parseInt(document.querySelector("#totalPrice").innerHTML.split("£")[1]);

RemoveBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault()

    console.log(totalPriceInner)

    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            let btnLink = btn.getAttribute("href")

            fetch(btnLink).then(function (result) {
                console.log(result.status)
                if (result.status == 200) {
                    let itemPrice = parseInt(document.querySelector("#itemPrice").innerHTML)

                    console.log(itemPrice)

                    btn.parentElement.parentElement.remove();
                    countField.innerHTML -= 1
                    totalPrice -= itemPrice

                    console.log(totalPrice)

                    totalPriceInner.innerHTML = totalPrice

                    Swal.fire({
                        title: "Deleted!",
                        text: "Your file has been deleted.",
                        icon: "success"
                    });
                }
                else {
                    Swal.fire({
                        title: "Something Went Wrong!",
                        text: "Please Try Again Later!",
                        icon: "error"
                    });
                }
            })

        }
    });
}))
