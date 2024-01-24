import React from "react";
import Link from "next/link";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faExternalLink } from "@fortawesome/free-solid-svg-icons";

export const ExternalLink = ({href, children, ...rest}: React.PropsWithChildren<{
    href: string
} & React.AnchorHTMLAttributes<HTMLAnchorElement>>) => (
    <Link {...rest}
          href={href}
          className={`bg-black accent-white p-1 px-2 radius ${rest.className}`}
          target="_blank">
        <FontAwesomeIcon icon={faExternalLink}/>
        <span className={"ml-2"}>{children}</span>
    </Link>
);